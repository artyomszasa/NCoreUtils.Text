namespace NCoreUtils.Text.Internal;

public static class ErrorCodeExtensions
{
	/// <summary>
	/// Determines whether the operation was successful or not.
	/// http://icu-project.org/apiref/icu4c/utypes_8h_source.html#l00709
	/// </summary>
	public static bool IsSuccess(this UErrorCode errorCode)
	{
		return errorCode <= UErrorCode.ZERO_ERROR;
	}

	/// <summary>
	/// Determines whether the operation resulted in an error.
	/// http://icu-project.org/apiref/icu4c/utypes_8h_source.html#l00714
	/// </summary>
	public static bool IsFailure(this UErrorCode errorCode)
	{
		return errorCode > UErrorCode.ZERO_ERROR;
	}
}