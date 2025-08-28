import React from "react";
import Hyperview, {
  HvComponentOnUpdate,
  HvComponentOptions,
  HvComponentProps,
  StyleSheets,
} from "hyperview";
import ReanimatedSwipeable from "react-native-gesture-handler/ReanimatedSwipeable";

const NAMESPACE_URI = "https://hypermedia.systems/hyperview/swipeable";

const getElements = (tagName: string, element: Element) => {
  return Array.from(element.getElementsByTagNameNS(NAMESPACE_URI, tagName));
};

const getButtons = (
  element: Element,
  stylesheets: StyleSheets,
  onUpdate: HvComponentOnUpdate,
  options: HvComponentOptions
) => {
  return getElements("button", element).map((buttonElement) => {
    return Hyperview.renderChildren(
      buttonElement,
      stylesheets,
      onUpdate,
      options
    );
  });
};

const SwipeableRow = (props: HvComponentProps) => {
  const [main] = getElements("main", props.element);
  if (!main) {
    return null;
  }

  return (
    <ReanimatedSwipeable
      renderRightActions={() =>
        getButtons(
          props.element,
          props.stylesheets,
          props.onUpdate,
          props.options
        )
      }
    >
      {Hyperview.renderChildren(
        main,
        props.stylesheets,
        props.onUpdate,
        props.options
      )}
    </ReanimatedSwipeable>
  );
};

SwipeableRow.namespaceURI = NAMESPACE_URI;
SwipeableRow.localName = "row";

export { SwipeableRow };
