import { fetchPostComment } from "../api/commentsApi";
import { useEffect, useState } from "react";

const useComment = (postId, commentId) => {
  const [comment, setComment] = useState(null);
  const [isLoading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!postId || !commentId) return;

      try {
        setLoading(true);
        const data = await fetchPostComment(postId, commentId);
        setComment(data);
        setError(null);
      } catch (err) {
        setError(err?.message || "Failed to fetch comment");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [postId, commentId]);

  return { comment, isLoading, error };
};

export default useComment;
