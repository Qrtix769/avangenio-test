namespace Api.Extensions;

public static class DateTimeExtension
{
	public static int GetAge(this DateTime dateOfBirth)
		=> DateTime.Now.Subtract(dateOfBirth).Days / 365;
}