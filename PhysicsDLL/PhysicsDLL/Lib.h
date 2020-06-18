#ifndef LIB_H
#define LIB_H

#ifdef PHYSICSDLL_EXPORT
	#define PHYSICSDLL_SYMBOL __declspec(dllexport)
#else //!PHYSICSDLL_EXPORT
	#ifdef PHYSICSDLL_IMPORT
		#define PHYSICSDLL_SYMBOL __declspec(dllimport)
	#else //!PHYSICSDLL_IMPORT
		#define PHYSICSDLL_SYMBOL
	#endif //PHYSICSDLL_IMPORT
#endif //PHYSICSDLL_EXPORT


#endif //!LIB_H