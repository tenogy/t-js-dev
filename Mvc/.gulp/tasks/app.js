var gulp = require("gulp");
var gutil = require("gulp-util");
var concat = require("gulp-concat");
var mergeStream = require("merge-stream");
var sass = require('gulp-sass');
var cssnano = require("gulp-cssnano");

var config = require("../config.js");
var bundle = require("../bundle");

var dir = config.app.dir;
var outcss = config.app.out + "css/";
var modules = config.app.modules;

gulp.task("app-build", ["app-css"]);
gulp.task("app-build-min", ["app-css-min"]);

gulp.task("app-css", function () {
	gutil.log(gutil.colors.blue("Build app css"));

	return buildAppCss();
});

gulp.task("app-css-min", function () {
	gutil.log(gutil.colors.blue("Minimazied app css"));
	return buildAppCss(function (s) {
		return s.pipe(cssnano({ discardComments: { removeAll: true } }));
	});
});

function buildAppCss(streamAction) {
	return config.app.modules.map(function (m) {
		//compile sass
		var sassStream = gulp.src(dir + m.dir + "/**/*.scss").pipe(sass().on('error', sass.logError));

		//select additional css files
		var cssStream = gulp.src(dir + m.dir + "/**/*.css");

		var mainStream = mergeStream(sassStream, cssStream);
		if (streamAction) {
			mainStream = streamAction(mainStream);
		}

		return mainStream.pipe(concat(m.dir + ".css")).pipe(gulp.dest(outcss));
	});
}