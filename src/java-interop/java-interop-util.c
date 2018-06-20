#ifdef WINDOWS
#include <assert.h>
#include <direct.h>
#include <stringapiset.h>

char*
utf16_to_utf8 (const wchar_t *widestr)
{
	int required_size = WideCharToMultiByte (CP_UTF8, 0, widestr, -1, NULL, 0, NULL, NULL);
	char *mbstr = calloc (required_size, sizeof (char));
	int converted_size = WideCharToMultiByte (CP_UTF8, 0, widestr, -1, mbstr, required_size, NULL, NULL);

	assert (converted_size == required_size);

	return mbstr;
}

wchar_t*
utf8_to_utf16 (const char *mbstr)
{
	int required_chars = MultiByteToWideChar (CP_UTF8, 0, mbstr, -1, NULL, 0);
	wchar_t *widestr = calloc (required_chars, sizeof (wchar_t));
	int converted_chars = MultiByteToWideChar (CP_UTF8, 0, mbstr, -1, widestr, required_chars);

	assert (converted_chars == required_chars);

	return widestr;
}
#endif // def WINDOWS