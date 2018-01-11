var map = function () {
    var words = this.body.split(' ');
	if(words){
		for (var i = 0; i < words.length; i++) {
			var value = {word: words[i], count:1}
			emit(this.author, value);
		}
	}
};

var reduce = function(key, values) {
	var rv = {
		words:[]
	};
	var words = [];
	var wordList = [];
	values.forEach( function(value) {
		if( words.indexOf(value.word) > -1){
		for(var i=0; i<wordList.length; i++){
			if(wordList[i].word === value.word)
				wordList[i].count += 1;
		}
		}else{
			words.push(value.word);
			wordList.push({word: value.word, count:value.count});
		}
	});
	wordList = wordList.sort( function(a,b){
		return b.count - a.count;
	});
	rv.words = wordList;
	return rv;
};

var fin = function(key, reducedVal){
	var counter = 1;
	var finVal = [];
	for(var i =0; i<reducedVal.words.length; i++){
		if(counter <11){
			finVal.push(reducedVal.words[i].word);
		}else{
			break;
		}
		counter +=1;
	}
	return finVal;
}

db.articles.mapReduce(
	map,
	reduce,
	{ out: "mr_author",
	 finalize:fin
	});
	
db.mr_author.find();	