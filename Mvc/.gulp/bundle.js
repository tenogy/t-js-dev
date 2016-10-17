var gulp = require("gulp");
var gulpif = require("gulp-if");
var gutil = require("gulp-util");
var uglify = require("gulp-uglify");
var sourcemaps = require("gulp-sourcemaps");
var cssnano = require("gulp-cssnano");
var concat = require("gulp-concat");
var sass = require('gulp-sass');

var config = require("./config.js");

var minType = {
	js: 1,
	css: 2,
	scss: 3
};

function joinSources(message, bundle, map, min) {
	gutil.log(gutil.colors.blue(message));
	var sources = bundle.src.map(function (src) { return src.dir ? src.dir + src.src : src.src });
	sources.forEach(function (path) { gutil.log(gutil.colors.blue(path)); });
	return gulp.src(sources).
		pipe(gulpif("*.scss", sass().on('error', sass.logError))).
		pipe(gulpif(map, sourcemaps.init())).
		pipe(concat(bundle.out)).
		pipe(gulpif(min === minType.js, uglify())).
		pipe(gulpif(min === minType.css, cssnano({ discardComments: { removeAll: true } }))).
		pipe(gulpif(map, sourcemaps.write("."))).
		pipe(gulp.dest(config.dist.root));
}

function copyAssets(from, to) {
	return gulp.src(from).
		pipe(gulp.dest(config.dist.root + to));
}

module.exports = {
	minType: minType,
	joinSources: joinSources,
	copyAssets: copyAssets
}

