
const express = require("express");
const fs = require("fs");
const path = require("path");

const app = express();
const PORT = 4444;


const PUBLIC_FOLDER = path.join(__dirname, "public");
const CONFIG_FILE = path.join(__dirname, "update.json");


const ARCHIVE_FILE = path.join(
    PUBLIC_FOLDER,
    "glitch_launcher.rar"
);


if (!fs.existsSync(PUBLIC_FOLDER)) {
    fs.mkdirSync(PUBLIC_FOLDER, { recursive: true });
}


app.get("/version.txt", (req, res) => {
    try {
        const config = JSON.parse(
            fs.readFileSync(CONFIG_FILE, "utf8")
        );

        if (!config.version) {
            return res.status(500).send("Version not configured.");
        }

        res.type("text/plain");
        res.send(String(config.version));
    }
    catch (error) {
        console.error("Version Error:", error.message);

        res.status(500).send("Unable to read version.");
    }
});


app.get("/glitch_launcher.rar", (req, res) => {
    if (!fs.existsSync(ARCHIVE_FILE)) {
        return res.status(404).send(
            "Glitch Launcher archive not found."
        );
    }

    res.download(
        ARCHIVE_FILE,
        "glitch_launcher.rar",
        (error) => {
            if (error) {
                console.error(
                    "Download Error:",
                    error.message
                );

                if (!res.headersSent) {
                    res.status(500).send(
                        "Unable to download launcher."
                    );
                }
            }
        }
    );
});


app.listen(PORT, "127.0.0.1", () => {
    console.log("-----------------------------------");
    console.log("     GLITCH LAUNCHER UPDATE SERVER");
    console.log("-----------------------------------");
    console.log("");
    console.log(`Server running on port ${PORT}`);
    console.log("");
    console.log("Version URL:");
    console.log(`http://localhost:${PORT}/version.txt`);
    console.log("");
    console.log("Download URL:");
    console.log(
        `http://localhost:${PORT}/glitch_launcher.rar`
    );
    console.log("");
    console.log("-----------------------------------");
});
