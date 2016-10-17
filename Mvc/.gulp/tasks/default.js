var gulp = require("gulp");
var runSequence = require("run-sequence");

gulp.task("default", ["vendor-all", "app-build"]);

gulp.task("rebuild", function (callback) {
	runSequence("clean", ["default"], callback);
});

gulp.task("release", function (callback) {
	runSequence("clean", ["vendor-all-min", "app-build-min"], callback);
});