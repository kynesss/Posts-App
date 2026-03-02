import React from "react";
import { useParams } from "react-router-dom";

const CommentEditPage = () => {
  const { postId, commentId } = useParams();

  return (
    <div>
      Post Id: {postId} Comment Id: {commentId}
    </div>
  );
};

export default CommentEditPage;
