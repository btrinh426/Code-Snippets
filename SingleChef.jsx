import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { useHistory } from "react-router-dom";
import * as userService from "@services/userService";
import debug from "sabio-debug";

const SingleChef = ({ chef }) => {
  const [chefInfo, setChefInfo] = useState([]);
  const history = useHistory();
  const _logger = debug.extend("SingleChef");

  useEffect(() => {
    userService
      .getCurrent()
      .then(onGetCurrentUserSuccess)
      .catch(onGetCurrentUserError);
  }, []);

  const onGetCurrentUserSuccess = (response) => {
    const chefData = response.item;
    setChefInfo(chefData);
  };

  const onGetCurrentUserError = (error) => {
    _logger("get currentUserId unsuccessful", error);
  };

  const editHandle = () => {
    history.push(`/profile/chef/${chef.id}/edit`, {
      type: "EDIT_CHEF",
      payload: chef,
    });
  };

  return (
    <div className="grid-item">
      <Grid item xs={12} lg={12}>
        <Card className="mb-4">
          <div className="card-img-wrapper">
            <div className="card-badges card-badges-bottom">
              {chef.statusId === "Active" ? (
                <div className="badge badge-success badge-pill">
                  {chef.statusId}
                </div>
              ) : (
                <div className="badge badge-danger badge-pill">
                  {chef.statusId}
                </div>
              )}
            </div>
            <img alt="profile picture" className="img" src={chef.avatarUrl} />
          </div>
          <div className="card-body text-center card-body-avatar">
            <div className="avatar-icon-wrapper d-120">
              <div className="avatar-icon rounded-circle">
                <img
                  alt="profile picture"
                  className="img-fluid"
                  src={chef.avatarThumbnailUrl}
                />
              </div>
            </div>
            <h3 className="font-weight-bold mt-1 mb-3">
              {chef.createdBy.firstName} {chef.createdBy.lastName}
            </h3>
            <p className="card-text mb-3">{chef.bio}</p>
            {chefInfo.id === chef.id ? (
              <Button
                className="m-2"
                variant="contained"
                color="primary"
                onClick={editHandle}
              >
                Edit
              </Button>
            ) : (
              <p></p>
            )}
          </div>
        </Card>
      </Grid>
    </div>
  );
};

SingleChef.propTypes = {
  chef: PropTypes.shape({
    createdBy: PropTypes.shape({
      firstName: PropTypes.string.isRequired,
      lastName: PropTypes.string.isRequired,
    }),
    id: PropTypes.number.isRequired,
    bio: PropTypes.string.isRequired,
    avatarThumbnailUrl: PropTypes.string.isRequired,
    avatarUrl: PropTypes.string.isRequired,
    statusId: PropTypes.string.isRequired,
  }),
};

export default SingleChef;
