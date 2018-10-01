const path = require("path");

function resolve(filePath) {
    return path.join(__dirname, filePath)
}


module.exports = {
    entry: [
        "isomorphic-fetch",
        './tests/Tests.fsproj'
    ],
    output: {
        filename: 'tests.bundle.js',
        path: resolve('./build'),
    },
    target: "node",
    mode: 'production',
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: "fable-loader",
            },
        ]
    }
};
