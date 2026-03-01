import {
  Box,
  Typography,
  Stack,
  Button,
  Alert,
  CircularProgress,
  TextField,
  Pagination,
  Container,
  Card,
  CardActionArea,
  CardContent,
} from "@mui/material";
import { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";

import usePostsMutations from "../hooks/usePostsMutations";
import usePost from "../hooks/usePost";

import { hoverCardSx } from "../styles/cardStyles";
import useComments from "../hooks/useComments";

const PostEditPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const {
    updatePost,
    isLoading: isSaving,
    error: saveError,
  } = usePostsMutations();
  const { post, isLoading: isFetching, error: fetchError } = usePost(id);
  const [editedPost, setEditedPost] = useState({ title: "", content: "" });

  const [pager, setPager] = useState({
    page: 1,
    pageSize: 10,
    sort: "id",
    order: "desc",
  });
  const [search, setSearch] = useState(null);
  const {
    comments,
    isLoading: isLoadingComments,
    error: fetchCommentsError,
  } = useComments(id, search, pager);

  useEffect(() => {
    if (!post) return;
    setEditedPost({
      title: post.title ?? "",
      content: post.content ?? "",
    });
  }, [post]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await updatePost(id, editedPost);
      navigate("/posts");
    } catch {
      //
    }
  };

  if (fetchError) {
    return <Alert severity="error">{fetchError}</Alert>;
  }

  if (isFetching)
    return (
      <Box sx={{ display: "flex", justifyContent: "center", py: 4 }}>
        <CircularProgress size={100} color="inherit" />
      </Box>
    );

  return (
    <Container>
      <Stack spacing={5} sx={{ m: "auto", maxWidth: 900 }}>
        <Box component="form" onSubmit={handleSubmit}>
          <Button
            variant="contained"
            size="large"
            component={Link}
            to="/posts"
            sx={{ width: 150, mb: 3 }}
          >
            Wróć
          </Button>

          {saveError && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {saveError}
            </Alert>
          )}

          <Stack spacing={4}>
            <TextField
              label="Tytuł"
              size="medium"
              value={editedPost.title}
              onChange={(e) =>
                setEditedPost({ ...editedPost, title: e.target.value })
              }
              fullWidth
              required
            />
            <TextField
              label="Treść"
              value={editedPost.content}
              onChange={(e) =>
                setEditedPost({ ...editedPost, content: e.target.value })
              }
              multiline
              minRows={5}
              fullWidth
              required
            />
            <Button
              type="submit"
              variant="contained"
              size="large"
              sx={{ width: 150 }}
              disabled={isSaving}
            >
              {isSaving ? (
                <CircularProgress size={22} color="inherit" />
              ) : (
                "Wyślij"
              )}
            </Button>
          </Stack>
        </Box>

        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            gap: 4,
          }}
        >
          <Typography variant="h4">Komentarze</Typography>
          <Stack spacing={2}>
            {comments.items.map((comment) => (
              <Box key={comment.id}>
                <Card sx={hoverCardSx}>
                  <CardContent>
                    <Typography variant="h6">{comment.content}</Typography>
                  </CardContent>
                </Card>
              </Box>
            ))}

            <Box sx={{ display: "flex" }}>
              <Pagination
                sx={{ m: "auto", maxWidth: "100%", mt: 2 }}
                page={pager.page}
                count={comments.totalPages ?? 1}
                color="primary"
                onChange={(_, value) =>
                  setPager((prev) => ({ ...prev, page: value }))
                }
              />
            </Box>
          </Stack>
        </Box>
      </Stack>
    </Container>
  );
};

export default PostEditPage;
