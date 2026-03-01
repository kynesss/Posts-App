import { getComments } from "../api/commentsApi";
import { useState, useEffect } from "react";

const useComments = (postId, search, pager) => {
  const [comments, setComments] = useState({
    items: [],
    totalPages: 1,
    page: 1,
  });
  const [isLoading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!postId) return;

      try {
        setLoading(true);
        const data = await getComments(postId, search, pager);
        setComments(data);
        setError(null);
      } catch (err) {
        setError(err?.message || "Failed to fetch comments");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [postId, search, pager]);

  return { comments, isLoading, error };
};

export default useComments;
