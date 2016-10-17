var gulp = require("gulp");
var config = require("../config.js");
var gutil = require("gulp-util");
var mergeStream = require("merge-stream");

var bundle = require("../bundle");

gulp.task("vendor-all", ["vendor-scripts", "vendor-styles", "vendor-assets"]);
gulp.task("vendor-all-min", ["vendor-scripts-min", "vendor-styles-min", "vendor-assets"]);

gulp.task("vendor-scripts", function() {
	return bundle.joinSources("Join the following scripts:", config.vendors.js, false);
});

gulp.task("vendor-scripts-min", function () {
	return bundle.joinSources("Join the following scripts:", config.vendors.js, false, bundle.minType.js);
});

gulp.task("vendor-styles", function () {
	return bundle.joinSources("Join the following styles:", config.vendors.css, false);
});

gulp.task("vendor-styles-min", function () {
	return bundle.joinSources("Join the following scripts:", config.vendors.css, false, bundle.minType.css);
});

gulp.task("vendor-assets", function () {
	gutil.log(gutil.colors.blue("Copy assets."));
	var streams = config.vendors.assets.map(function (a) { return bundle.copyAssets(a.src, a.out); });

	return mergeStream(streams);
});
