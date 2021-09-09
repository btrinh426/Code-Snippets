import axios from "axios";
import {
  API_HOST_PREFIX,
  onGlobalSuccess,
  onGlobalError,
} from "./serviceHelpers";

let endpoint = API_HOST_PREFIX + "/api/chefs/";

let getChefs = (pageIndex, pageSize) => {
  const config = {
    method: "GET",
    url: endpoint + `paginate?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export { getChefs };
