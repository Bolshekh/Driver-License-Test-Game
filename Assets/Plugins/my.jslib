mergeInto(LibraryManager.library, {

GetLang: function () {
    	try {
      	var lang = YaGames.init().then(ysdk => ysdk.environment.i18n.lang);
      	var bufferSize = lengthBytesUTF8(lang) + 1;
      	var buffer = _malloc(bufferSize);
      	stringToUTF8(lang, buffer, bufferSize);
      	return buffer;
    	} catch(err){
      	// взять язык с браузера
      	return navigator.language;
    	}
    	},


});