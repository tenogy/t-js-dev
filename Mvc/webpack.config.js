module.exports = {
	entry: {
		"list/main": __dirname + "/src/list/main.ts",
		"select2/main": __dirname + "/src/select2/main.ts"
	},

	//devtool: "source-map",

	resolve: {
		extensions: ["", ".webpack.js", ".web.js", ".ts", ".tsx", ".js"]
	},

	module: {
		loaders: [
			{ test: /\.json$/, loader: "json-loader" },
			{ test: /\.tsx?$/, loader: "ts-loader?silent=true" }
		]
	},

	output: {
		path: __dirname + "/content/app/",
		filename: "[name].js",
		libraryTarget: "umd",
		library: ["_app_", "[name]"]
	},
	externals: {
		"tenogy": "tenogy"
	}
};
