import { addPost } from "../api/postsApi";
import { useState } from "react";

const useCreatePost = () => {
  const [isLoading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const createPost = async (post) => {
    try {
      setLoading(true);
      setError(null);
      const data = await addPost(post);
      return data;
    } catch (err) {
      setError(err?.message || "Failed to add post");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { createPost, isLoading, error };
};

export default useCreatePost;
