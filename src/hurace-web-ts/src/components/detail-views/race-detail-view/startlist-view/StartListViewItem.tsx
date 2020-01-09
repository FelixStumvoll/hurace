import React from 'react';
import { StartList } from '../../../../models/StartList';
import { TextBold } from '../../../../theme/CustomComponents';

export const StartListViewItem: React.FC<{ startList: StartList }> = ({
    startList
}) => {
    return (
        <tr>
            <td>
                <TextBold>{startList.startNumber}</TextBold>
            </td>
            <td>{startList.skier.country?.countryCode}</td>
            <td>
                {startList.skier.firstName} {startList.skier.lastName}
            </td>
        </tr>
    );
};
