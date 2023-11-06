#include <windows.h>
#include <stdio.h>
int main(int argc, char ** argv) {
	int sec = 1;
	if (argc > 1) sec = atoi(argv[1]);
	printf("pid: %d, sleep for %d seconds\n",getpid(), sec);
	Sleep(sec * 1000 ); 
	return -1;
}
