import { addPost, editPost, removePost } from "../api/postsApi";
import { useState } from "react";

const usePostsMutations = () => {
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

  const createPost = (post) => run(() => addPost(post));
  const updatePost = (id, post) => run(() => editPost(id, post));
  const deletePost = (id) => run(() => removePost(id));

  return { createPost, updatePost, deletePost, isLoading, error };
};

export default usePostsMutations;
