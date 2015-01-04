#include <Windows.h>
#include <Wincon.h>
#include <iostream>

using namespace std;


void SetRusFont() {

	CONSOLE_FONT_INFOEX cfon;
	ZeroMemory(&cfon, sizeof(CONSOLE_FONT_INFOEX));
	cfon.cbSize = sizeof(CONSOLE_FONT_INFOEX);
	cfon.nFont = 6;
	cfon.dwFontSize.X = 7;
	cfon.dwFontSize.Y = 16;
	cfon.FontFamily = 54; // Lucida Console
	cfon.FontWeight = 400;
	lstrcpyW(cfon.FaceName, L"Lucida Console");
	SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), false, &cfon);

	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);
}
