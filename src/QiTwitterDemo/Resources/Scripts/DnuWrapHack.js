var fs = require("fs");
var path = require("path");
var filesList = [];
var pathPrefix = "../../../";
var pathSuffix = "";
var pathSep = new RegExp("[\\\\\/]", "g");
module.exports = {
    changeReferencePaths: function () {
        var wrapFolder = "../../wrap/";
        console.log("Script Path: " + __dirname);
        //iterate through the project.json
        module.exports.walkPathSync(wrapFolder);
        console.log(filesList);
        console.log("------------------------------------------------------------------------------------------");
        //read, modify and overwrite the json accordingly (a dirty hack)
        //keep tabs on the latest dnx release to see if older project reference support is improved
        filesList.forEach(function (file) {
            var projectJson = JSON.parse(fs.readFileSync(file, 'utf8'));
            var projName = path.basename(projectJson.frameworks.net46.bin.assembly).split(".");
            var tmpPathName = "";
            var cnt = 0;
            if (projName[0].toLowerCase() == "osisoft" || projName[0].toLowerCase() == "interop") {
                pathSuffix = "";
                if (projName[2].toLowerCase() == "helper") {
                    tmpPathName = projectJson.frameworks.net46.wrappedProject.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    if (pathSuffix.toLowerCase().search("common") <= 0) {
                        pathSuffix = "/Common/OSIsoft.Qi.Helper/" + pathSuffix;
                    }
                    projectJson.frameworks.net46.wrappedProject = pathPrefix + pathSuffix;

                    pathSuffix = "";
                    tmpPathName = projectJson.frameworks.net46.bin.assembly.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    if (pathSuffix.toLowerCase().search("common") <= 0) {
                        pathSuffix = "/Common/OSIsoft.Qi.Helper/" + pathSuffix;
                    }
                    projectJson.frameworks.net46.bin.assembly = pathPrefix + pathSuffix;

                    pathSuffix = "";
                    tmpPathName = projectJson.frameworks.net46.bin.pdb.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    if (pathSuffix.toLowerCase().search("common") <= 0) {
                        pathSuffix = "/Common/OSIsoft.Qi.Helper/" + pathSuffix;
                    }
                    projectJson.frameworks.net46.bin.pdb = pathPrefix + pathSuffix;
                    console.log("---------------------------------------------------------------------------------------------------------------------------------");
                    console.log(projectJson.frameworks.net46.dependencies);
                    if("Interop.WUApiLib" in projectJson.frameworks.net46.dependencies){
                        projectJson.frameworks.net46.dependencies["Interop.WUApiLib"] = "2.0.0-*";
                    }
                    pathSuffix = "";
                }
                else if(projName[2].toLowerCase() == "fabric") {
                    tmpPathName = projectJson.frameworks.net46.wrappedProject.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    projectJson.frameworks.net46.wrappedProject = pathPrefix + pathSuffix;

                    pathSuffix = "";
                    tmpPathName = projectJson.frameworks.net46.bin.assembly.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    projectJson.frameworks.net46.bin.assembly = pathPrefix + pathSuffix;

                    pathSuffix = "";
                    tmpPathName = projectJson.frameworks.net46.bin.pdb.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    projectJson.frameworks.net46.bin.pdb = pathPrefix + pathSuffix;

                    pathSuffix = "";
                }
                else {
                    tmpPathName = projectJson.frameworks.net46.bin.assembly.split(pathSep);
                    tmpPathName = tmpPathName.filter(Boolean);
                    tmpPathName.forEach(function (txt, index, arr) { module.exports.pathAppend(txt, index, arr) });
                    if (pathSuffix.toLowerCase().search("common") <= 0) {
                        pathSuffix = "/Common/OSIsoft.Qi.Helper/OSIsoft.Qi.Helper/" + pathSuffix;
                    }
                    if (pathSuffix.toLowerCase().search("debug") >= 0) {
                        pathSuffix = pathSuffix.replace("debug", "{configuration}");
                    }
                    projectJson.frameworks.net46.bin.assembly = pathPrefix + pathSuffix;
                    projectJson.version = "2.0.0-*";

                    pathSuffix = "";
                }
                try {
                    fs.writeFileSync(file, JSON.stringify(projectJson, null, 4), "utf8");
                } catch (err) {
                    console.log(err);
                }
            }
        });
    },

    walkPathSync : function (dirPath) {
        try {
            var files = fs.readdirSync(dirPath);
        }
        catch (err) {
            console.log(err);
        }
        files.forEach(function (file) {
            var fileFullPath = path.join(dirPath, file);
            var fileStat = fs.statSync(fileFullPath);
            if (fileStat.isDirectory()) {
                module.exports.walkPathSync(fileFullPath);
            }
            else {
                filesList.push(fileFullPath);
            }
        });
    },

    pathAppend: function (txt, index, arr) {
        if (txt != "..") {
            if (index != (arr.length - 1)) {
                pathSuffix = pathSuffix + "/" + txt + "/";
            }
            else {
                pathSuffix = pathSuffix + "/" + txt;
            }
        }
    }
};