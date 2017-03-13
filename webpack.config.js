var path = require('path');
var fs = require('fs');
var child_process = require("child_process");

var babelOptions = {
  "presets": [
    [path.resolve(__dirname, "node_modules/babel-preset-es2015"), {"modules": false}]
  ]
}

module.exports = {
  entry: './tests/Tests.fsproj',
  output: {
    filename: 'tests.bundle.js',
    path: path.resolve(__dirname, 'build'),
  },
  target: "node",
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: { babel: babelOptions }
        }
      },
      {
        test: /\.js$/,
        exclude: /node_modules\/(?!fable)/,
        use: {
          loader: 'babel-loader',
          options: babelOptions
        },
      }
    ]
  },
};
