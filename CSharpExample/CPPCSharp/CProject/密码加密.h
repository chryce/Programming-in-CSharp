#pragma once
#define  _CRT_SECURE_NO_WARNINGS//�رհ�ȫ���
#include<stdio.h>
#include<stdlib.h>
#include<string.h>

//���ܣ���������

// �ļ�����
// �ַ�������
char * stringjiami(char *password, char *string);
//�ַ�������
char * stringjiemi(char *password, char *jiastring);
int  getfilesize(char * path);
void filejiami(char *path, char *pathjia, char *jiapassword[]);
void filejiemi(char *pathjia, char *pathjie, char *jiapassword[]);
