module.exports = {
  entry: './tests/Tests.fsproj',
  output: {
    filename: 'tests.bundle.js',
    path: './build',
  },
  target: "node",
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: "fable-loader"
      },
    ]
  },
};
