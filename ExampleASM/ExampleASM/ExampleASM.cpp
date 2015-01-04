#include "rusfont.h"
#include <stdlib.h>
#include <stdio.h>

int NumAverage(int n) {
	int x, i, cur_num;
	char *_str_buf = new char[255], *cur_num_str = new char[255];
	_asm {
		mov eax, 0;
		mov x, eax;
		mov eax, 0;
		mov i, eax;
	labelFOR8:
		mov eax, i;
		push eax;
		mov eax, n;
		pop ebx;
		cmp eax, ebx;
		jg labelCMP16;
		mov eax, 0;
		jmp labelCMP17;
	labelCMP16:
		mov eax, 1;
	labelCMP17:
		cmp eax, 0;
		je labelFOR9;
		push cur_num_str;
		call gets;
		add esp, 4;
		mov cur_num_str, eax;
		mov eax, cur_num_str;
		push eax;
		call atoi;
		add esp, 4;
		mov cur_num, eax;
		mov eax, x;
		push eax;
		mov eax, cur_num;
		pop ebx;
		add eax, ebx;
		mov x, eax;
	labelFOR10:
		mov eax, i;
		push eax;
		mov eax, 1;
		pop ebx;
		add eax, ebx;
		mov i, eax;
		jmp labelFOR8;
	labelFOR9:
		mov eax, x;
		push eax;
		mov eax, n;
		pop ebx;
		mov ecx, eax;
		mov eax, ebx;
		mov edx, 0;
		idiv ecx;
		mov x, eax;
		mov eax, x;
		jmp labelRET1;
	labelRET1:
	}
}




void QuadEquationTry(int a, int b, int c) {
	int d;
	char *_str_buf = new char[255];
	char *_str_0 = "Нет действительных корней";
	char *_str_1 = "Существует 1 корень";
	char *_str_2 = "Существует 2 различных корня";
	_asm {
		mov eax, b;
		push eax;
		mov eax, b;
		pop ebx;
		imul ebx;
		push eax;
		mov eax, 4;
		push eax;
		mov eax, a;
		pop ebx;
		imul ebx;
		push eax;
		mov eax, c;
		pop ebx;
		imul ebx;
		pop ebx;
		sub eax, ebx;
		neg eax;
		mov d, eax;
		mov eax, d;
		push eax;
		mov eax, 0;
		pop ebx;
		cmp eax, ebx;
		jg labelCMP51;
		mov eax, 0;
		jmp labelCMP52;
	labelCMP51:
		mov eax, 1;
	labelCMP52:
		cmp eax, 0;
		je labelIF47;
		mov eax, _str_0;
		push eax;
		call puts;
		add esp, 4;
		jmp labelIF48;
	labelIF47:
		mov eax, d;
		push eax;
		mov eax, 0;
		pop ebx;
		cmp eax, ebx;
		je labelCMP55;
		mov eax, 0;
		jmp labelCMP56;
	labelCMP55:
		mov eax, 1;
	labelCMP56:
		cmp eax, 0;
		je labelIF53;
		mov eax, _str_1;
		push eax;
		call puts;
		add esp, 4;
		jmp labelIF54;
	labelIF53:
		mov eax, _str_2;
		push eax;
		call puts;
		add esp, 4;
	labelIF54:
	labelIF48 :
	labelRET41 :
	}
}




void main() {

	SetRusFont();
	int variant, num, result, a, b, c;
	char *_str_buf = new char[255], *variant_string = new char[255], *num_string = new char[255], *result_string = new char[255], *a_str = new char[255], *b_str = new char[255], *c_str = new char[255];
	char *_str_3 = "Выберите функцию:";
	char *_str_4 = "1. Определить среднее арифметическое нескольких чисел";
	char *_str_5 = "2. Проверка на существование корней квадратного уравнения";
	char *_str_6 = "Введите количество чисел:";
	char *_str_7 = "Введите числа:";
	char *_str_8 = "Ответ:";
	char *_str_9 = "Введите 3 коэффицента:";
	_asm {
		mov eax, _str_3;
		push eax;
		call puts;
		add esp, 4;
		mov eax, _str_4;
		push eax;
		call puts;
		add esp, 4;
		mov eax, _str_5;
		push eax;
		call puts;
		add esp, 4;
		push variant_string;
		call gets;
		add esp, 4;
		mov variant_string, eax;
		mov eax, variant_string;
		push eax;
		call atoi;
		add esp, 4;
		mov variant, eax;
		mov eax, variant;
		push eax;
		mov eax, 1;
		pop ebx;
		cmp eax, ebx;
		je labelCMP76;
		mov eax, 0;
		jmp labelCMP77;
	labelCMP76:
		mov eax, 1;
	labelCMP77:
		cmp eax, 0;
		je labelIF74;
		mov eax, _str_6;
		push eax;
		call puts;
		add esp, 4;
		push num_string;
		call gets;
		add esp, 4;
		mov num_string, eax;
		mov eax, num_string;
		push eax;
		call atoi;
		add esp, 4;
		mov num, eax;
		mov eax, _str_7;
		push eax;
		call puts;
		add esp, 4;
		mov eax, num;
		push eax;
		call NumAverage;
		add esp, 4;
		mov result, eax;
		mov eax, result;
		push 10;
		push result_string;
		push eax;
		call _itoa;
		add esp, 8;
		add esp, 4;
		mov result_string, eax;
		mov eax, _str_8;
		push eax;
		call puts;
		add esp, 4;
		mov eax, result_string;
		push eax;
		call puts;
		add esp, 4;
		jmp labelIF75;
	labelIF74:
		mov eax, variant;
		push eax;
		mov eax, 2;
		pop ebx;
		cmp eax, ebx;
		je labelCMP116;
		mov eax, 0;
		jmp labelCMP117;
	labelCMP116:
		mov eax, 1;
	labelCMP117:
		cmp eax, 0;
		je labelIF114;
		mov eax, _str_9;
		push eax;
		call puts;
		add esp, 4;
		push a_str;
		call gets;
		add esp, 4;
		mov a_str, eax;
		push b_str;
		call gets;
		add esp, 4;
		mov b_str, eax;
		push c_str;
		call gets;
		add esp, 4;
		mov c_str, eax;
		mov eax, a_str;
		push eax;
		call atoi;
		add esp, 4;
		mov a, eax;
		mov eax, b_str;
		push eax;
		call atoi;
		add esp, 4;
		mov b, eax;
		mov eax, c_str;
		push eax;
		call atoi;
		add esp, 4;
		mov c, eax;
		mov eax, a;
		push eax;
		mov eax, b;
		push eax;
		mov eax, c;
		push eax;
		call QuadEquationTry;
		add esp, 12;
	labelIF114:
	labelIF75 :
		push _str_buf;
			  call gets;
			  add esp, 4;
		  labelRET60:
	}
}




