using LargeXMLAnalyzer;

var options = ArgumentHandler.HandleArguments(args);

OptionsHandler.TakeInitialAction(options);

var validatedOption = OptionsHandler.ValidateOptions(options);

OptionsHandler.HandleOptions(validatedOption);

