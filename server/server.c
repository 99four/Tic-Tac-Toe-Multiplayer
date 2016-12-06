#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <signal.h>
#include <sys/wait.h>
#include <pthread.h>
#include <stdlib.h>

//-lpthread
struct cln {
  int cfd;
  struct sockaddr_in caddr;
};

struct Player {
  int descriptor;
  char* nickname;
};

struct Game {
  struct Player x;
  struct Player o;
  char arrayOfTurns[9];
  int turnCounter;
};

struct Player *playersQueue[100];
struct Game *gamesQueue[100];

int currentPlayersLength = 0;
int currentGamesLength = 0;

struct Player* returnOpponent() {
  return playersQueue[--currentPlayersLength];
}

char checkIfTheresAWinner(char arrayOfTurns[9], int turnCounter) {
  char arrayOfConditions[8][3] = {
                { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, // horizontal conditions
                { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, // vertical conditions
                { 0, 4, 8 }, { 2, 4, 6 } // diagonal conditions
        };

  int i,j;

  for (i = 0; i < 8; i++) {
    int isWinner = 1;
    char start = arrayOfTurns[arrayOfConditions[i][0]];
    if (start == 'n') {
      continue;
    }

    for (j = 1; j < 3; j++) {
      if  (start != arrayOfTurns[arrayOfConditions[i][j]]) {
        isWinner = 0;
      }
    }

    if (isWinner == 1) {
      return start;
    }

  }

  if (turnCounter == 9) {
    return 'd';
  }
  return 'n';
}

int returnProperArrayIndex(char* msg) {
  if (strcmp(msg, "a1") == 0) {
    return 0;
  }
  else if (strcmp(msg, "a2") == 0) {
    return 1;
  }
  else if (strcmp(msg, "a3") == 0) {
    return 2;
  }
  else if (strcmp(msg, "b1") == 0) {
    return 3;
  }
  else if (strcmp(msg, "b2") == 0) {
    return 4;
  }
  else if (strcmp(msg, "b3") == 0) {
    return 5;
  }
  else if (strcmp(msg, "c1") == 0) {
    return 6;
  }
  else if (strcmp(msg, "c2") == 0) {
    return 7;
  }
  else if (strcmp(msg, "c3") == 0) {
    return 8;
  }
  return -1;
}

void addToQueue(struct Player *p) {
  playersQueue[currentPlayersLength] = p;
  printf("add to queue %s\n", playersQueue[currentPlayersLength]->nickname);
  currentPlayersLength++;
}

void createNewGame(struct Player *p1, struct Player *p2) {
   struct Game* myGame;
   myGame = malloc(sizeof(struct Game));
   myGame->x = *p2;
   myGame->o = *p1;
   gamesQueue[currentGamesLength] = myGame;
   memset(myGame->arrayOfTurns, 'n', sizeof(myGame->arrayOfTurns));
   printf("tworze nowa gre, imiona graczy to %s %s\n", myGame->o.nickname, myGame->x.nickname);
   myGame->turnCounter = 0;
   currentGamesLength++;
}

struct Game* returnMyGame(struct Player *p) {
  printf("szukam gry, moj deskryptor to: %d\n", p->descriptor);
  int i;
  for (i = 0; i < currentGamesLength; i++) {
    if (gamesQueue[i]->x.descriptor == p->descriptor || gamesQueue[i]->o.descriptor == p->descriptor) {
      return gamesQueue[i];
    }
  }
}

void* cthread(void* arg) {
  printf("nowy watek\n");
  struct cln* c = (struct cln*)arg;
  struct Player* myPlayer;
  myPlayer = malloc(sizeof(struct Player));
  struct Player* myOpponent;
  myOpponent = malloc(sizeof(struct Player));

  myPlayer->descriptor = c->cfd;

  char msgFromClient[100];
  char myTurn;

  int code = 1;
  while(code != 4) {
    memset(msgFromClient, 0, 100);
    int iter;

    read(c->cfd, msgFromClient, sizeof(msgFromClient));
    char decodedMsg[100];
    memset(decodedMsg, 0, 100);
    code = 0;
    sscanf(msgFromClient, "%d %s", &code, decodedMsg);
    myPlayer->nickname = strdup(decodedMsg);
    if(code == 4) {
      printf("code %d\n", code);
    }
    switch(code) {
      case 1:
      {
        if (currentPlayersLength == 0) {
          myTurn = 'X';
          printf("dodaje do kolejki\n");
          addToQueue(myPlayer);
        } else {
          struct Player* opponnent = returnOpponent();
          memcpy(myOpponent, opponnent, sizeof(struct Player));
          char messageToMe[100];
          char messageToOpponnent[100];
          createNewGame(myPlayer, myOpponent);

          sprintf(messageToMe, "%s %d 1", myOpponent->nickname, myOpponent->descriptor); // 1 - my turn, 0 - opponnent's turn
          sprintf(messageToOpponnent, "%s %d 0", myPlayer->nickname, myPlayer->descriptor); // begin with: 1 - O, 0 - X

          myTurn = 'O';
          write(c->cfd, messageToMe, sizeof(messageToMe));
          write(myOpponent->descriptor, messageToOpponnent, sizeof(messageToOpponnent));
        }
        break;
      }
      case 2: // ruch gracza
      {
        int opponentDescriptor;
        sscanf(msgFromClient, "%d %s %d", &code, decodedMsg, &opponentDescriptor);

        struct Game* myGame = returnMyGame(myPlayer);

        int arrIndexToFill = returnProperArrayIndex(decodedMsg);
        printf("indeks tablicy to %d\n", arrIndexToFill);

        myGame->arrayOfTurns[arrIndexToFill] = myTurn;

        // zawartosc obecnej tablicy ruchow graczy
        int i;
        for (i = 0; i < 9; i++) {
          printf("%c\n", myGame->arrayOfTurns[i]);
        }

        printf("ruszam pionkiem na pole %s, deskryptor klienta to %d\n", decodedMsg, opponentDescriptor);

        myGame->turnCounter++;

        char winner = checkIfTheresAWinner(myGame->arrayOfTurns, myGame->turnCounter);
        if (winner != 'n' && winner != 'd') {
          printf("mamy zwyciezce: %c\n", winner);
          char winnerMessageToMe[100];
          char winnerMessageToOpponnent[100];
          sprintf(winnerMessageToMe, "4 wygral %c", winner);
          sprintf(winnerMessageToOpponnent, "4 wygral %c %s", winner, decodedMsg);
          printf("wiadomosc do przeciwnika to %s\n", winnerMessageToOpponnent);
          write(opponentDescriptor, winnerMessageToOpponnent, sizeof(winnerMessageToOpponnent));
          write(c->cfd, winnerMessageToMe, sizeof(winnerMessageToMe));
        }
        else {
          char goOnMessage[100];
          sprintf(goOnMessage, "2 %s", decodedMsg);
          write(opponentDescriptor, goOnMessage, sizeof(goOnMessage));
        }
        break;
      }
    }
  }
  close(c->cfd);
  free(c);
  return 0;
}

int main(int args, char** argv)
{
  printf("server dziala\n");
  pthread_t tid;
  socklen_t slt;
  int nFoo = 1;
  int sd = socket(PF_INET, SOCK_STREAM, 0);
  setsockopt(sd, SOL_SOCKET, SO_REUSEADDR, (char*)&nFoo, sizeof(nFoo));
  struct sockaddr_in addr;
  addr.sin_family = PF_INET;
  addr.sin_port = htons(6669);
  addr.sin_addr.s_addr = INADDR_ANY;
  bind(sd, (struct sockaddr*)&addr, sizeof(addr));
  listen(sd, 10);

  while(1)
  {
    struct cln* c = malloc(sizeof(struct cln));
    slt = sizeof(c->caddr);
    c->cfd = accept(sd, (struct sockaddr*)&c->caddr, &slt);
    pthread_create(&tid, NULL, cthread, c);
    pthread_detach(tid);
  }
  return 0;
}
