var path = require("path");

module.exports = {
    entry: path.join(__dirname, "."),
    outDir: path.join(__dirname, "../build"),
    allFiles: true,
    babel: {
        plugins: ["@babel/plugin-transform-modules-commonjs"]
    }
}
