var map = function(){
	if(this.comments !== undefined){
		emit(this._id, this.comments.length);
	}
	else{
		emit(this,_id, 0);
	}
}

var reduce = function(key, values) {
	var rv = {
		comment : key,
		count : 0
	};
	values.forEach( function(value) {
		rv.count += value.count;
	});
	return rv;
}

db.nmbp.mapReduce(
	map,
	reduce,
	{	out: "mr_comments"
	});
	
db.mr_comments.find().sort({value:-1});