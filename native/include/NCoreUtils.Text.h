#ifndef __NCOREUTILS_TEXT__
#define __NCOREUTILS_TEXT__
#include "common.h"

typedef enum
{
    FormC = 0x1,
    FormD = 0x2,
    FormKC = 0x5,
    FormKD = 0x6
} NormalizationForm;

DECLEXPORT
const UNormalizer2*
ncoreutils_text_get_normalizer(NormalizationForm normalizationForm, UErrorCode* err);

// DECLEXPORT
// int32_t
// ncoreutils_text_normalize(const UNormalizer2* pNormalizer, const UChar* lpSrc, int32_t cwSrcLength, UChar* lpDst, int32_t cwDstLength, UErrorCode* err)

DECLEXPORT
int32_t
ncoreutils_text_decompose(const UNormalizer2* pNormalizer, const UChar32 c, UChar *decomposition, int32_t capacity, UErrorCode* err);

#endif