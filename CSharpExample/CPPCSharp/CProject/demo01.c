#define  _CRT_SECURE_NO_WARNINGS//�رհ�ȫ���
#include"�������.h"

void   main01()
{
	char *path = "C:\\Users\\jie\\Desktop\\ԭ.txt";
	char *pathjia = "C:\\Users\\jie\\Desktop\\����.txt";
	char *pathjie = "C:\\Users\\jie\\Desktop\\����.txt";
	char *jiapassword[20] = { 0 };
	char *jiepassword[20] = { 0 };
	printf("������������룺");
	scanf("%s", &jiapassword);

	printf("%s\n", jiapassword);
		filejiami(path, pathjia, jiapassword);
		printf("������������룺");
	AA:
		scanf("%s", &jiepassword);
		if (*jiapassword != *jiepassword)
		{
			printf("�������벻һ�£�����������������룺");
			goto AA;
		}
		filejiami(pathjia, pathjie, jiepassword);
		printf("���ܳɹ���");
	system("pause");
}