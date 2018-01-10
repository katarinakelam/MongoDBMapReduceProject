var map = function () {
    var words = this.body.split(' ');
    for (var i = 0; i < words.length; i++) {
        emit(this.author, words[i]);
    }
};

var reduce = function(key, values) {
	var rv = {
		words:[]
	};
	//imamo listu riječi da znamo kada se počnu ponavljati
	var words = [];
	//dictionary koji ima {riječ, ponavljanje} u sebi
	var wordList = [];
	values.forEach( function(value) {
		if( words.indexOf(value.word) == -1){ //ne postoji u listi
			words.push(value.word);
			wordList.push({word: value.word, count:value.count});
		}
		else{ //već postoji u listi
			for(var i=0; i<wordList.length; i++){
				if(wordList[i].word === value.word){
					wordList[i].count += 1;
				}
			}
		}
	});
	//soritramo listu tako da su najčešće riječi na vrhu
	wordList = wordList.sort( function(elementA,elementB){ //parametri prema kojima sort radi, lamdba f-ja, u ovom slucaju prema vecem countu
		return elementB.count - elementA.count;
	});
	rv.words = wordList
	return rv;
};

var fin = function (key, reducedVal) {
    var rv = [];
    for (var i = 0; i <= 10; i++) {
        rv.push(reducedVal.words[i].word);
    }
    return rv;
}

db.nmbp.mapReduce(
	map,
	reduce,
	{ out: "mr_author",
	finalize:fin
	});
	
db.mr_author.find();	