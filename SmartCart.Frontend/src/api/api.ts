import axios from "axios";

const API = axios.create({
  baseURL: "https://localhost:7180/api", // as per backend launchsettings.json
});

export default API;