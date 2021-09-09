import React, { useState, useEffect } from "react";
import * as chefsService from "@services/chefsService";
import { useHistory } from "react-router-dom";
import debug from "sabio-debug";

const AllChefs = () => {
  const [chef, setChef] = useState([]);
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize] = useState(4);
  const [totalCount, setTotalCount] = useState(0);
  const [searchChef, setSearchChef] = useState("");
  const history = useHistory();
  const _logger = debug.extend("AllChefs");

  useEffect(() => {
    chefsService
      .getChefs(pageIndex, pageSize)
      .then(onGetChefsSuccess)
      .catch(onGetChefsError);
  }, [pageIndex]);

  const onGetChefsSuccess = (response) => {
    const chefData = response.item.pagedItems;
    const mappedChefs = chefData.map(mapSingleChef);
    setChef(mappedChefs);

    const totalCount = response.item.totalCount;
    setTotalCount(totalCount);
  };

  const onGetChefsError = (err) => {
    _logger("unsuccessful", err);
  };

  const mapSingleChef = (chef) => <SingleChef chef={chef} key={chef.id} />;

  return <div>{chef}</div>;
};

export default AllChefs;
