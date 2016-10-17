var del = require("del");
var gulp = require("gulp");
var config = require("../config.js");


gulp.task("clean", function () {
	del.sync([config.dist.root, config.build.root]);
});