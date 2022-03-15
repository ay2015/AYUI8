namespace Xceed.Wpf.Toolkit.Core.Input
{
	/// <summary>Provides members related to input validation.</summary>
	public interface IValidateInput
	{
		event InputValidationErrorEventHandler InputValidationError;

		bool CommitInput();
	}
}
