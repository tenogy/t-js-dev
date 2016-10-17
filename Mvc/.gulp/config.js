module.exports = {
	dist: {
		root: "./static/"
	},
	build: {
		root: "./build/"
	},
	app: {
		dir: "./.client_src/app/",
		out: "./static/",
		modules: [
			{ dir: "list" },
			{ dir: "select2" }
		]
	},
	vendors: {
		js: {
			out: "js/common.js",
			src: [
				{ dir: "./node_modules/jquery/dist/", src: "jquery.js", min: "jquery.min.js", map: "jquery.min.map" },
				{ dir: "./node_modules/bootstrap/dist/js/", src: "bootstrap.js", min: "bootstrap.min.js" },
				{ dir: "./node_modules/knockout/build/output/", src: "knockout-latest.debug.js", min: "knockout-latest.js" },
				{ dir: "./node_modules/knockout.validation/dist/", src: "knockout.validation.js", min: "knockout.validation.min.js", map: "knockout.validation.min.js.map" },
				{ dir: "./node_modules/spin.js/", src: "spin.js", min: "spin.min.js" },
				{ dir: "./node_modules/select2/dist/js/", src: "select2.js", min: "select2.min.js" },
				{ dir: "./node_modules/rx/dist/", src: "rx.lite.js", min: "rx.lite.min.js", map: "rx.lite.map" },
				{ src: "./node_modules/tenogy/dist/tenogy.js" },
				{ src: "./.client_src/common/js/*" }
			]
		},
		css: {
			out: "css/common.css",
			src: [
				{ src: "./node_modules/bootstrap/dist/css/bootstrap.css" },
				{ src: "./node_modules/select2/dist/css/select2.css" },
				{ src: "./.client_src/common/css/*" }
			]
		},
		assets: [
			{ out: "fonts", src: "./node_modules/bootstrap/dist/fonts/*" }
		]
	}
}