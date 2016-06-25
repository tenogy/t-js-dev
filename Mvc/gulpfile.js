var gulp = require("gulp");
var del = require("del");
var browserify = require("browserify");
var source = require("vinyl-source-stream");
var tsify = require("tsify");
var ts = require("gulp-typescript");
var watchify = require("watchify");
var gutil = require("gulp-util");
var sourcemaps = require("gulp-sourcemaps");
var buffer = require("vinyl-buffer");
var typedoc = require("gulp-typedoc");
var packageJson = require("./package.json");
var uglify = require("gulp-uglify");
var rename = require("gulp-rename");
var concat = require("gulp-concat");
var streamify = require("gulp-streamify");
var mergeStream = require("merge-stream");

gulp.task("lib", function () {
	return gulp.src([
			"./node_modules/jquery/dist/jquery.js",
			"./node_modules/knockout/build/output/knockout-latest.debug.js",
			"./node_modules/knockout.validation/dist/knockout.validation.js",
			"./node_modules/rx/dist/rx.lite.js",
			"./node_modules/spin.js/spin.js",
			"./node_modules/almond/almond.js",
			"./node_modules/tenogy/dist/tenogy.js"
		])
		.pipe(concat("common.js"))
		.pipe(gulp.dest("./content/js"));
});

gulp.task("clean", function () {
	return del(["./dev/*"]);
});

gulp.task("default", ["lib"]);