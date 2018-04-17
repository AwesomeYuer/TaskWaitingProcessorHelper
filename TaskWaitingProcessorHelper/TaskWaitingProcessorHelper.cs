namespace Microshaoft
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Forms;

    public interface IDialogResultForm
    {
        void SetDialogResultProcess(params DialogResult[] results);
    }


    public static class TaskWaitingProcessorHelper
    {
        public static void ProcessWaitingShowDialogWindow
                    (
                        //IWin32Window ownerWindow
                        Window dialogWindow
                        , Action<Window> onProcessAction = null
                        , Func<Exception, Window, DialogResult> onCaughtExceptionProcessFunc = null
                    )
        {
            DialogResult r = default(DialogResult);
            var IsCompleted = false;
            if (onProcessAction != null)
            {
                var thread = new Thread
                        (
                            new ThreadStart
                                (
                                    () =>
                                    {
                                        //wait.WaitOne();
                                        Thread.Sleep(10);
                                        try
                                        {
                                            onProcessAction(dialogWindow);
                                            IsCompleted = true;
                                        }
                                        catch (Exception e)
                                        {
                                            //r = -1;
                                            if (onCaughtExceptionProcessFunc != null)
                                            {
                                                var rr = onCaughtExceptionProcessFunc(e, dialogWindow);
                                                //IDialogResultForm form = dialogForm as IDialogResultForm;
                                                //form.SetDialogResultProcess(rr);

                                            }
                                        }
                                        finally
                                        {
                                            //TrySafeFormInvokeClose
                                            //    (
                                            //        dialogForm
                                            //        , onCaughtExceptionProcessFunc
                                            //    );

                                            dialogWindow
                                                .Dispatcher
                                                .Invoke
                                                    (
                                                        () =>
                                                        {
                                                            dialogWindow.Close();
                                                        }
                                                    );
                                        }
                                    }
                                )
                        );
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                //wait.Set();
                if (!IsCompleted)
                {
                     dialogWindow.ShowDialog();
                }
            }
            //return r;
        }


        public static DialogResult ProcessWaitingShowDialog
                    (
                        IWin32Window ownerWindow
                        , Form dialogForm
                        , Action<Form> onProcessAction = null
                        , Func<Exception, Form, DialogResult> onCaughtExceptionProcessFunc = null
                    )
        {
            //var wait = new AutoResetEvent(false);
            DialogResult r = default(DialogResult);
            var IsCompleted = false;
            if (onProcessAction != null)
            {
                var thread = new Thread
                        (
                            new ThreadStart
                                (
                                    () =>
                                    {
                                        //wait.WaitOne();
                                        Thread.Sleep(10);
                                        try
                                        {
                                            onProcessAction(dialogForm);
                                            IsCompleted = true;
                                        }
                                        catch (Exception e)
                                        {
                                            //r = -1;
                                            if (onCaughtExceptionProcessFunc != null)
                                            {
                                                var rr = onCaughtExceptionProcessFunc(e, dialogForm);
                                                //IDialogResultForm form = dialogForm as IDialogResultForm;
                                                //form.SetDialogResultProcess(rr);

                                            }
                                        }
                                        finally
                                        {
                                            //TrySafeFormInvokeClose
                                            //    (
                                            //        dialogForm
                                            //        , onCaughtExceptionProcessFunc
                                            //    );
                                        }
                                    }
                                )
                        );
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                //wait.Set();
                if (!IsCompleted)
                {
                    r = dialogForm.ShowDialog(ownerWindow);
                }
            }
            return r;
        }


        public static bool TrySafeInvoke
                                (
                                    this Form target
                                    , Action<Form> action
                                    , Func<Exception, Form, DialogResult> onCaughtExceptionProcessFunc = null
                                )
        {
            var r = TrySafeInvokeFormAction
                        (
                            target
                            , action
                            , onCaughtExceptionProcessFunc
                        );
            return r;
        }
        public static bool TrySafeInvokeClose
                                (
                                    this Form target
                                    , Func<Exception, Form, DialogResult> onCaughtExceptionProcessFunc = null
                                )
        {
            var r = TrySafeInvokeFormClose
                        (
                            target
                            , onCaughtExceptionProcessFunc
                        );
            return r;
        }

        public static bool TrySafeInvokeFormAction
                                        (
                                            Form dialogForm
                                            , Action<Form> invokeAction
                                            , Func<Exception, Form, DialogResult> onCaughtExceptionProcessFunc = null
                                        )
        {
            bool r = false;
            try
            {
                if
                    (
                        dialogForm.IsHandleCreated
                        && !dialogForm.IsDisposed
                    )
                {
                    dialogForm
                            .Invoke
                                (
                                    new Action
                                    (
                                        () =>
                                        {
                                            invokeAction(dialogForm);
                                        }
                                    )
                                );
                    Thread.Sleep(10);
                }
                r = true;
            }
            catch (Exception e)
            {
                r = false;
                if (onCaughtExceptionProcessFunc != null)
                {
                    var rr = onCaughtExceptionProcessFunc(e, dialogForm);
                }
            }
            return r;
        }


        public static bool TrySafeInvokeWindowAction
                                (
                                    Window dialogWindow
                                    , Action<Window> invokeAction
                                    , Func<Exception, Window, DialogResult> onCaughtExceptionProcessFunc = null
                                )
        {
            bool r = false;
            try
            {
                dialogWindow
                        .Dispatcher
                        .Invoke
                            (
                                //new Action
                                //(
                                    () =>
                                    {
                                        invokeAction(dialogWindow);
                                    }
                                //)
                            );
                Thread.Sleep(10);
                r = true;
            }
            catch (Exception e)
            {
                r = false;
                if (onCaughtExceptionProcessFunc != null)
                {
                    var rr = onCaughtExceptionProcessFunc(e, dialogWindow);
                }
            }
            return r;
        }


        public static bool TrySafeInvokeFormClose
                                (
                                    Form dialogForm
                                    , Func<Exception, Form, DialogResult> onCaughtExceptionProcessFunc = null
                                )
        {
            var action = new Action<Form>
                            (
                                (x) =>
                                {
                                    if
                                        (
                                            dialogForm.IsHandleCreated
                                            && !dialogForm.IsDisposed
                                        )
                                    {
                                        dialogForm.Close();
                                    }
                                }
                            );
            bool r = TrySafeInvokeFormAction
                            (
                                dialogForm
                                , action
                                , onCaughtExceptionProcessFunc
                            );
            return r;
        }


        public static bool TrySafeInvokeWindowClose
                                (
                                    Window dialogWindow
                                    , Func<Exception, Window, DialogResult> onCaughtExceptionProcessFunc = null
                                )
        {
            var action = new Action<Window>
                            (
                                (x) =>
                                {
                                    //if
                                    //    (
                                    //        dialogWindow.IsHandleCreated
                                    //        && !dialogWindow.IsDisposed
                                    //    )
                                    //{
                                        dialogWindow.Close();
                                    //}
                                }
                            );
            bool r = TrySafeInvokeWindowAction
                            (
                                dialogWindow
                                , action
                                , onCaughtExceptionProcessFunc
                            );
            return r;
        }

    }
}
