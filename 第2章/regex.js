var pattern = /(.)+/g;
var input = "abcd";
var match = pattern.exec(input);
console.log(match.length);
console.log(match[0]);
console.log(match[1]);