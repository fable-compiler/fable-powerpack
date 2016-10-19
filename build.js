var path = require("path");
var fs = require("fs-extra");
var fable = require("fable-compiler");

function promise(f) {
    args = Array.from(arguments).slice(1);
    return new Promise(function (resolve, reject) {
        f.apply(this, args.concat(function (err, data) {
            if (err)
                reject(err);
            else
                resolve(data);
        }))
    });
}

function runCommand(workingDir, command) {
    var child_process = require('child_process');
    function splitByWhitespace(str) {
        function stripQuotes(str, start, end) {
            return str[start] === '"' && str[end - 1] === '"'
                    ? str.substring(start + 1, end - 1)
                    : str.substring(start, end);
        }
        var reg = /\s+(?=([^"]*"[^"]*")*[^"]*$)/g;
        reg.lastIndex = 0;
        var tmp, tmp2, results = [], lastIndex = 0;
        while ((tmp = reg.exec(str)) !== null) {
            results.push(stripQuotes(str, lastIndex, tmp.index));
            lastIndex = tmp.index + tmp[0].length;
        }
        results.push(stripQuotes(str, lastIndex, str.length));
        return results;
    }

    var cmd, args;
    process.stdout.write(workingDir + "> " + command + "\n");
    if (process.platform === "win32") {
        cmd = "cmd";
        args = splitByWhitespace(command);
        args.splice(0,0,"/C");
    }
    else {
        args = splitByWhitespace(command);
        cmd = args[0];
        args = args.slice(1);
    }

    var proc = child_process.spawn(cmd, args, { cwd: workingDir });
    return new Promise(function (resolve, reject) {
        proc.on('exit', function(code) {
            if (code === 0)
                resolve();
            else
                reject(code);
        });
        proc.stderr.on('data', function(data) {
            process.stderr.write(data.toString());
        });
        proc.stdout.on('data', function(data) {
            process.stdout.write(data.toString());
        });
    });
}

var targets = {
    All() {
        promise(fs.remove, "npm")
            .then(_ => promise(fs.remove, "build"))
            .then(_ => fable({ projFile: "." }))
            .then(_ => fable({ projFile: ".", target: "next" }))
            .then(_ => promise(fs.copy, "package.json", "npm/package.json"))
            .then(_ => promise(fs.copy, "README.md", "npm/README.md"))
            .then(_ => promise(fs.readFile, "RELEASE_NOTES.md"))
            .then(line => {
                var version = /\d[^\s]*/.exec(line)[0];
                return runCommand("npm", "npm version " + version);
            })
            .catch(err => {
                console.log("[ERROR] " + err);
                proccess.exit(-1);
            })
    }
}

targets[process.argv[2] || "All"]();
