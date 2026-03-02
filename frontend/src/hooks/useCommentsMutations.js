import { addComment, editComment, deleteComment } from "../api/commentsApi";
import { useState } from "react";

const useCommentsMutations = () => {
  const [isLoading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const run = async (action) => {
    try {
      setLoading(true);
      setError(null);
      return await action();
    } catch (err) {
      setError(err?.message || "Request failed");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const createComment = (postId, content) =>
    run(() => addComment(postId, content));
  const updateComment = (id, content) => run(() => editComment(id, content));
  const removeComment = (id) => run(() => deleteComment(id));

  return { createComment, updateComment, removeComment, isLoading, error };
};

export default useCommentsMutations;
