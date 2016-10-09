var gulp = require("gulp");
var del = require("del");
var concat = require("gulp-concat");

gulp.task("lib", function () {
	return gulp.src([
			"./node_modules/jquery/dist/jquery.js",
			"./node_modules/knockout/build/output/knockout-latest.debug.js",
			"./node_modules/knockout.validation/dist/knockout.validation.js",
			"./node_modules/rx/dist/rx.lite.js",
			"./node_modules/spin.js/spin.js",
			"./node_modules/tenogy/dist/tenogy.js"
		])
		.pipe(concat("common.js"))
		.pipe(gulp.dest("./content/"));
});

gulp.task("clean", function () {
	return del(["./dev/*"]);
});

gulp.task("default", ["lib"]);