#include"�������.h"

int  getfilesize(char * path)
{
	FILE *pf = fopen(path, "r");
	if (pf == NULL)
	{
		return -1;
	}
	else
	{
		fseek(pf, 0, SEEK_END);//�ļ�ĩβ
		int length = ftell(pf);
		return length;//���س���
	}
}

char * stringjiami(char *password, char *string)
{
	int passlength = strlen(password);//��ȡ���ܳ���
	int stringlength = strlen(string);//��ȡ�ַ�������
	if (stringlength %passlength == 0)
	{
		int ci = stringlength / passlength;//ѭ������
		for (int i = 0; i < ci; i++)//ѭ����
		{
			for (int j = 0; j < passlength; j++)//ѭ������
			{
				string[passlength*i + j] ^= password[j];//������
			}
		}
	}
	else
	{
		int ci = stringlength / passlength;//ѭ������
		for (int i = 0; i < ci; i++)//ѭ����
		{
			for (int j = 0; j < passlength; j++)//ѭ������
			{
				string[passlength*i + j] ^= password[j];//������
			}
		}
		int lastlength = stringlength%passlength;//ʣ�µĳ���
		for (int i = 0; i < lastlength; i++)
		{
			string[passlength*(stringlength / passlength) + i] ^= password[i];
		}
	}
	return  string;
}
//�ַ�������
char * stringjiemi(char *password, char *jiastring)
{
	int passlength = strlen(password);//��ȡ���ܳ���
	int stringlength = strlen(jiastring);//��ȡ�ַ�������
	if (stringlength %passlength == 0)
	{
		int ci = stringlength / passlength;//ѭ������
		for (int i = 0; i < ci; i++)//ѭ����
		{
			for (int j = 0; j < passlength; j++)//ѭ������
			{
				jiastring[passlength*i + j] ^= password[j];//������
			}
		}
	}
	else
	{
		int ci = stringlength / passlength;//ѭ������
		for (int i = 0; i < ci; i++)//ѭ����
		{
			for (int j = 0; j < passlength; j++)//ѭ������
			{
				jiastring[passlength*i + j] ^= password[j];//������
			}
		}
		int lastlength = stringlength%passlength;//ʣ�µĳ���
		for (int i = 0; i < lastlength; i++)
		{
			jiastring[passlength*(stringlength / passlength) + i] ^= password[i];
		}
	}
	return  jiastring;
}

void filejiami(char *path, char *pathjia, char *password)
{
	FILE *pfr, *pfw;
	pfr = fopen(path, "rb");
	pfw = fopen(pathjia, "wb");
	if (pfr == NULL || pfw == NULL)
	{
		fclose(pfr);
		fclose(pfw);
		return;
	}
	else
	{
		int length = getfilesize(path);//��ȡ����   430
									   //int passlength = strlen(password);//��ȡ����20
		char *newstr = (char*)malloc(sizeof(char)*(length + 1));
		for (int i = 0; i < length; i++)
		{
			char ch = fgetc(pfr);//��ȡһ���ַ�
			newstr[i] = ch;//���ϴ����ַ�
		}
		newstr[length] = '\0';//�ַ�������Ϊ'\0'
		stringjiami(password, newstr);//�����ַ���
		for (int i = 0; i < length; i++)
		{
			fputc(newstr[i], pfw);//����д���ַ�
		}
		fclose(pfr);
		fclose(pfw);//�ر��ļ�
		free(newstr);
	}
}

void filejiemi(char *pathjia, char *pathjie, char *password[])
{
	FILE *pfr;
	FILE *pfw;
	pfr = fopen(pathjia, "rb");
	pfw = fopen(pathjie, "wb");
	if (pfr == NULL || pfw == NULL)
	{
		fclose(pfr);
		fclose(pfw);
		return;
	}
	else
	{
		while (!feof(pfr))
		{
			char str[1024] = { 0 };
			fgets(str, 1024, pfr);
			stringjiemi(password, str);//���ܰ�
			fputs(str, pfw);//д��jie���ļ�
		}
		fclose(pfr);
		fclose(pfw);//�ر��ļ�
	}
}