var ExtractTextPlugin = require("extract-text-webpack-plugin");

module.exports = {
	entry: {
		"list": __dirname + "/.client_src/app/list/main.ts",
		"select2": __dirname + "/.client_src/app/select2/main.ts"
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
		path: __dirname + "/static/js/",
		filename: "[name].js",
		libraryTarget: "umd",
		library: ["_app_", "[name]"]
	},

	externals: {
		"tenogy": "tenogy"
	}
};
