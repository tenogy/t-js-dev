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

var tsProject = ts.createProject("./src/tsconfig.json");

var tsifyConfig = {
	"noImplicitAny": false,
	"noEmitOnError": true,
	"removeComments": true,
	"sourceMap": true,
	"target": "es5",
	"module": "amd",
	"experimentalDecorators": true,
	"emitDecoratorMetadata": true
};

function bundle(entry, dest) {
	return browserify({
		basedir: ".",
		debug: true,
		entries: ["src/index.d.ts", entry],
		cache: {},
		packageCache: {}
		}).plugin(tsify, tsifyConfig).
		bundle().
		on("error", gutil.log).
		pipe(source(dest)).
		pipe(buffer()).
		pipe(sourcemaps.init({ loadMaps: true })).
		pipe(sourcemaps.write("./")).
		pipe(gulp.dest("content"));
}

gulp.task("pre-compile", ["clean"], function () {
	return bundle("src/container.ts", "container.js");
});

gulp.task("compile", ["pre-compile"], function () {
	var streams = [];

	streams.push(bundle("src/list/main.ts", "list/main.js"));

	streams.push(gulp.src([
			"node_modules/jquery/dist/jquery.js",
			"node_modules/knockout/build/output/knockout-latest.debug.js",
			"node_modules/knockout.validation/dist/knockout.validation.js",
			"node_modules/rx/dist/rx.lite.js",
			"node_modules/spin.js/spin.js",
			"node_modules/tenogy/dist/tenogy.js",
			//"node_modules/requirejs/require.js",
			//"src/require-config.js",
			"content/container.js"
			]).
		pipe(concat("common.js")).
		pipe(gulp.dest("./content/")));

	return mergeStream(streams);
});

gulp.task("clean", function () {
	return del(["./content/*"]);
});

gulp.task("default", ["compile"]);