var path = require("path");
var fs = require("fs-extra");
var fable = require("fable-compiler");

function promise(f) {
    args = Array.from(arguments).slice(1);
    return new Promise(function (resolve, reject) {
        f.apply(this, args.concat(function (err, data) {
            if (err) { reject(err); } else { resolve(data); }
        }))
    });
}

var targets = {
    All() {
        promise(fs.remove, "npm")
            .then(_ => promise(fs.remove, "build"))
            .then(_ => fable.compile())
            .then(_ => fable.compile({ target: "next" }))
            .then(_ => promise(fs.copy, "package.json", "npm/package.json"))
            .then(_ => promise(fs.copy, "README.md", "npm/README.md"))
            .then(_ => promise(fs.readFile, "RELEASE_NOTES.md"))
            .then(line => {
                var version = /\d[^\s]*/.exec(line)[0];
                return fable.runCommand("npm", "npm version " + version);
            })
            .catch(err => {
                console.log("[ERROR] " + err);
                proccess.exit(-1);
            })
    }
}

targets[process.argv[2] || "All"]();
