#include <unicode/unorm2.h>
#include "NCoreUtils.Text.h"

DECLEXPORT
const UNormalizer2*
ncoreutils_text_get_normalizer(NormalizationForm normalizationForm, UErrorCode* err)
{
    const UNormalizer2* pNormalizer = NULL;
    switch (normalizationForm)
    {
        case FormC:
            pNormalizer = unorm2_getNFCInstance(err);
            break;
        case FormD:
            pNormalizer = unorm2_getNFDInstance(err);
            break;
        case FormKC:
            pNormalizer = unorm2_getNFKCInstance(err);
            break;
        case FormKD:
            pNormalizer = unorm2_getNFKDInstance(err);
            break;
        default:
            *err = U_ILLEGAL_ARGUMENT_ERROR;
            break;
    }
    return pNormalizer;
}

// DECLEXPORT
// int32_t
// ncoreutils_text_normalize(const UNormalizer2* pNormalizer, const UChar* lpSrc, int32_t cwSrcLength, UChar* lpDst, int32_t cwDstLength, UErrorCode* err)
// {
//     int32_t normalizedLen = unorm2_normalize(pNormalizer, lpSrc, cwSrcLength, lpDst, cwDstLength, err);
//     return normalizedLen
// }

DECLEXPORT
int32_t
ncoreutils_text_decompose(const UNormalizer2* pNormalizer, const UChar32 c, UChar *decomposition, int32_t capacity, UErrorCode* err)
{
    uint32_t decompositionLen = unorm2_getDecomposition(pNormalizer, c, decomposition, capacity, err);
    return decompositionLen;
}